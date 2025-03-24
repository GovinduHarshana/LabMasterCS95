const express = require("express");
const router = express.Router();
const bcrypt = require("bcryptjs");
const User = require("../../models/user");

// Login Route
router.post("/login", async (req, res) => {
    try {
        const { email, password } = req.body;

        // Validate input
        if (!email || !password) {
            return res.status(400).json({ message: "Email and password are required" });
        }

        // Find user in the database
        const user = await User.findOne({ email });

        if (!user) {
            return res.status(401).json({ message: "Invalid email or password" });
        }

        // Compare password
        const isMatch = await bcrypt.compare(password, user.passwordHash);
        if (!isMatch) {
            return res.status(401).json({ message: "Invalid email or password" });
        }

        // Return user data
        res.status(200).json({
            message: "Login successful",
            userId: user._id,
            name: user.name,
            role: user.userRole
        });

    } catch (error) {
        console.error("Login error:", error);
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;
