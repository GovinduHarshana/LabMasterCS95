const express = require("express");
const router = express.Router();
const bcrypt = require("bcryptjs");
const User = require("../../models/user");

router.post("/signup", async (req, res) => {
    try {
        const { email, name, dob, userRole, password, rePassword } = req.body;

        if (!email || !name || !dob || !userRole || !password || !rePassword) {
            return res.status(400).json({ message: "All fields are required" });
        }
        if (!isValidEmail(email)) 
            return res.status(400).json({ message: "Invalid email format" });

        if (password !== rePassword) 
            return res.status(400).json({ message: "Passwords do not match" });

        if (!isValidPassword(password)) 
            return res.status(400).json({ message: "Weak password" });

        const existingUser = await User.findOne({ email });
        if (existingUser) return res.status(400).json({ message: "User already exists" });

        const hashedPassword = await bcrypt.hash(password, 10);
        const newUser = new User({ email, name, dob, userRole, passwordHash: hashedPassword });
        await newUser.save();

        res.status(201).json({ message: "User registered successfully" });
    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;
