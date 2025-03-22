const express = require("express");
const mongoose = require("mongoose");
const User = require("../../models/user");
const router = express.Router();
const bcrypt = require("bcrypt");

// GET Profile
router.get("/:id", async (req, res) => {
    console.log("Incoming GET request for user ID:", req.params.id);
    try {
        if (!mongoose.Types.ObjectId.isValid(req.params.id)) {
            return res.status(400).json({ message: "Invalid user ID" });
        }

        const user = await User.findById(req.params.id);
        if (!user) {
            return res.status(404).json({ message: "User not found" });
        }

        res.status(200).json(user);
    } catch (error) {
        console.error("Error fetching profile:", error.message);
        res.status(500).json({ message: "Error fetching profile", error: error.message });
    }
});

// PUT Profile - Update
router.put("/update/:id", async (req, res) => {
    console.log("Incoming PUT request for user ID:", req.params.id, "with data:", req.body);
    try {
        const { name, dob, mobileNumber, nic, password } = req.body;

        if (!mongoose.Types.ObjectId.isValid(req.params.id)) {
            return res.status(400).json({ message: "Invalid user ID" });
        }

        let user = await User.findById(req.params.id);
        if (!user) {
            return res.status(404).json({ message: "User not found" });
        }

        if (mobileNumber && mobileNumber.length !== 10) {
            return res.status(400).json({ message: "Mobile number must be 10 digits." });
        }

        if (nic && nic.length !== 12) {
            return res.status(400).json({ message: "NIC must be 12 digits." });
        }

        user.name = name || user.name;
        user.dob = dob || user.dob;
        user.mobileNumber = mobileNumber || user.mobileNumber;
        user.nic = nic || user.nic;

        if (password) {
            const salt = await bcrypt.genSalt(10);
            const hashedPassword = await bcrypt.hash(password, salt);
            user.password = hashedPassword;
        }

        await user.save();
        res.status(200).json({ message: "Profile updated successfully", user });
    } catch (error) {
        console.error("Error updating profile:", error.message);
        res.status(500).json({ message: "Error updating profile", error: error.message });
    }
});

module.exports = router;