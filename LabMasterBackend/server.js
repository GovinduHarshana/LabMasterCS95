require("dotenv").config();
const express = require("express");
const mongoose = require("mongoose");
const cors = require("cors");
const bcrypt = require("bcryptjs");
const nodemailer = require("nodemailer");
const crypto = require("crypto");
const User = require("./models/User");

const app = express();
app.use(express.json());
app.use(cors());

// MongoDB Connection
const PORT = process.env.PORT || 5000;
const MONGO_URI = process.env.MONGO_URI;

mongoose.connect(MONGO_URI, { useNewUrlParser: true, useUnifiedTopology: true })
  .then(() => console.log("✅ MongoDB connected"))
  .catch((err) => {
    console.error("❌ MongoDB connection error:", err);
    process.exit(1);
  });

// Nodemailer Transporter
const transporter = nodemailer.createTransport({
    service: "gmail",
    secure: true,
    port: 465,
    auth: {
        user: process.env.EMAIL_USER,
        pass: process.env.EMAIL_PASS
    }
});

// Password Validation Function
const isValidPassword = (password) => {
    return /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(password);
};

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
        if (!isValidPassword(password)) {
            return res.status(400).json({ message: "Weak password: Must be 8+ characters with 1 uppercase, 1 number, and 1 special character." });
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
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Login API
app.post("/login", async (req, res) => {
    try {
        const { email, password } = req.body;
        const user = await User.findOne({ email });

        if (!user || !(await bcrypt.compare(password, user.passwordHash))) {
            return res.status(400).json({ message: "Invalid email or password" });
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
        if (!email) return res.status(400).json({ message: "Email is required." });
        
        const user = await User.findOne({ email });
        if (!user) return res.status(404).json({ message: "User not found." });

        const resetToken = crypto.randomBytes(20).toString("hex");
        user.resetToken = resetToken;
        user.resetTokenExpiry = Date.now() + 3600000;
        await user.save();

        const resetLink = `${process.env.FRONTEND_URL}/reset-password?token=${resetToken}`;
        const mailOptions = {
            from: `LabMaster Support <${process.env.EMAIL_USER}>`,
            to: email,
            subject: "Reset Your Password - LabMaster",
            html: `<p>Click <a href='${resetLink}'>here</a> to reset your password. This link expires in 1 hour.</p>`
        };

        await transporter.sendMail(mailOptions);
        res.json({ message: "Check your email for the reset link!" });
    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Reset Password API
app.post("/reset-password", async (req, res) => {
    try {
        const { token, newPassword } = req.body;
        if (!isValidPassword(newPassword)) {
            return res.status(400).json({ message: "Weak password: Must be 8+ characters with 1 uppercase, 1 number, and 1 special character." });
        }
        
        const user = await User.findOne({ resetToken: token, resetTokenExpiry: { $gt: Date.now() } });
        if (!user) return res.status(400).json({ message: "Invalid or expired token" });

        user.passwordHash = await bcrypt.hash(newPassword, 10);
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
        return res.status(400).json({ message: "Invalid JSON format" });
    }
    next();
});

// Start Server
app.listen(PORT, () => {
    console.log(`🚀 Server running on port ${PORT}`);
});
