require("dotenv").config();
const express = require("express");
const mongoose = require("mongoose");
const cors = require("cors");
const bcrypt = require("bcryptjs");
const jwt = require("jsonwebtoken");
const nodemailer = require("nodemailer");
const crypto = require("crypto");

const app = express();
app.use(express.json());
app.use(cors());

// Set up the port and MongoDB URI from environment variables
const PORT = process.env.PORT || 5000;
const MONGO_URI = process.env.MONGO_URI || "mongodb+srv://labmasterAdmin:LabmasterCS95@labmastercluster.urqii.mongodb.net/LabMaster";

// Connect to MongoDB
mongoose.connect(MONGO_URI, {})
  .then(() => console.log("✅ MongoDB connected"))
  .catch((err) => {
    console.error("❌ MongoDB connection error:", err);
    process.exit(1); // Terminate if connection fails
  });

// User Schema
const UserSchema = new mongoose.Schema({
    email: { type: String, required: true, unique: true },
    name: { type: String, required: true },
    dob: { type: String, required: true },
    userRole: { type: String, required: true, enum: ["Student", "Teacher", "Institute", "Other"] },
    passwordHash: { type: String, required: true },
    resetToken: { type: String, default: null },
    resetTokenExpiry: { type: Date, default: null },
    createdAt: { type: Date, default: Date.now }
});

const User = mongoose.model("User", UserSchema);

// Nodemailer Transporter (Update with real credentials)
const transporter = nodemailer.createTransport({
    service: "gmail",
    secure: true, // Use SSL
    port: 465, // Change from 587 to 465
    auth: {
        user: process.env.EMAIL_USER,
        pass: process.env.EMAIL_PASS
    }
});


// Signup API
app.post("/signup", async (req, res) => {
    try {
        const { email, name, dob, userRole, password, rePassword } = req.body;

        if (!email || !name || !dob || !userRole || !password || !rePassword) {
            return res.status(400).json({ message: "All fields are required" });
        }

        if (password !== rePassword) {
            return res.status(400).json({ message: "Passwords do not match" });
        }

        const existingUser = await User.findOne({ email });
        if (existingUser) {
            return res.status(400).json({ message: "User already exists" });
        }

        const hashedPassword = await bcrypt.hash(password, 10);
        const newUser = new User({ email, name, dob, userRole, passwordHash: hashedPassword });

        await newUser.save();
        res.status(201).json({ message: "User registered successfully" });

    } catch (error) {
        console.error(error);
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Login API
app.post("/login", async (req, res) => {
    try {
        const { email, password } = req.body;

        const user = await User.findOne({ email });
        if (!user) {
            return res.status(400).json({ message: "User not found" });
        }

        const isMatch = await bcrypt.compare(password, user.passwordHash);
        if (!isMatch) {
            return res.status(400).json({ message: "Incorrect password" });
        }

        res.status(200).json({ message: "Login successful" });

    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Forgot Password API
app.post("/forgot-password", async (req, res) => {
    try {
        const { email } = req.body;
        const user = await User.findOne({ email });

        if (!user) {
            return res.status(404).json({ message: "User not found" });
        }

        // Generate Reset Token (expires in 1 hour)
        const resetToken = crypto.randomBytes(20).toString("hex");
        user.resetToken = resetToken;
        user.resetTokenExpiry = Date.now() + 3600000; 

        await user.save();

        // Send Email with Reset Link
        const resetLink = `http://localhost:3000/reset-password?token=${resetToken}`;
        const mailOptions = {
            from: process.env.EMAIL_USER,
            to: email,
            subject: "Password Reset Request",
            text: `Click this link to reset your password: ${resetLink}`
        };

        transporter.sendMail(mailOptions, (error, info) => {
            if (error) {
                console.error("❌ Email error:", error);
                return res.status(500).json({ message: "Failed to send email", error: error.message });
            }
            console.log("✅ Email sent:", info.response);
            res.json({ message: "Password reset email sent" });
        });      

    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Reset Password API
app.post("/reset-password", async (req, res) => {
    try {
        const { token, newPassword } = req.body;
        const user = await User.findOne({ resetToken: token, resetTokenExpiry: { $gt: Date.now() } });

        if (!user) {
            return res.status(400).json({ message: "Invalid or expired token" });
        }

        const hashedPassword = await bcrypt.hash(newPassword, 10);
        user.passwordHash = hashedPassword;
        user.resetToken = null;
        user.resetTokenExpiry = null;

        await user.save();
        res.json({ message: "Password has been reset successfully" });

    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Middleware to catch invalid JSON
app.use((err, req, res, next) => {
    if (err instanceof SyntaxError) {
        return res.status(400).json({ message: "Invalid JSON" });
    }
    next();
});

// Start Server
app.listen(PORT, () => {
    console.log(`🚀 Server running on port ${PORT}`);
});
