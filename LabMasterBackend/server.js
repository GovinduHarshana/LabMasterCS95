require("dotenv").config();
const express = require("express");
const mongoose = require("mongoose");
const cors = require("cors");
const bcrypt = require("bcryptjs");
const jwt = require("jsonwebtoken");


const app = express();
app.use(express.json());
app.use(cors());

// Set up the port and MongoDB URI from environment variables
const PORT = process.env.PORT || 5000;
const MONGO_URI = process.env.MONGO_URI || "mongodb+srv://labmasterAdmin:LabmasterCS95@labmastercluster.urqii.mongodb.net/LabMaster";

// Connect to MongoDB with improved error handling
mongoose.connect(MONGO_URI, {})
  .then(() => console.log("MongoDB connected"))
  .catch((err) => {
    console.error("MongoDB connection error:", err);
    process.exit(1); // Terminate the process if MongoDB connection fails
  });

// User Schema
const UserSchema = new mongoose.Schema({
    email: { type: String, required: true, unique: true },
    name: { type: String, required: true },
    dob: { type: String, required: true },
    userRole: { type: String, required: true, enum: ["Student", "Teacher", "Institute", "Other"] },
    passwordHash: { type: String, required: true },
    createdAt: { type: Date, default: Date.now }
});

const User = mongoose.model("User", UserSchema);

// Signup API
app.post("/signup", async (req, res) => {
    try {
        const { email, name, dob, userRole, password, rePassword } = req.body;

        // Validate input fields
        if (!email || !name || !dob || !userRole || !password || !rePassword) {
            return res.status(400).json({ message: "All fields are required" });
        }

        // Check if passwords match
        if (password !== rePassword) {
            return res.status(400).json({ message: "Passwords do not match" });
        }

        // Check if user already exists
        const existingUser = await User.findOne({ email });
        if (existingUser) {
            return res.status(400).json({ message: "User already exists" });
        }

        // Hash the password before saving
        const hashedPassword = await bcrypt.hash(password, 10);
        const newUser = new User({ email, name, dob, userRole, passwordHash: hashedPassword });

        await newUser.save();
        res.status(201).json({ message: "User registered successfully" });

    } catch (error) {
        console.error(error);  // Log the error for debugging
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Login API
app.post("/login", async (req, res) => {
    try {
        const { email, password } = req.body;
        const user = await User.findOne({ email });

        // Validate credentials
        if (!user || !(await bcrypt.compare(password, user.passwordHash))) {
            return res.status(400).json({ message: "Invalid credentials" });
        }

        // Sign JWT token
        const token = jwt.sign({ userId: user._id }, process.env.JWT_SECRET || "your_secret_key", { expiresIn: "1h" });
        res.status(200).json({ token, userRole: user.userRole });
    } catch (error) {
        console.error(error);  // Log the error for debugging
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

// Start the server
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
