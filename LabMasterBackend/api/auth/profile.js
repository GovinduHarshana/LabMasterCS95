const express = require("express");
const mongoose = require("mongoose");
const User = require("../../models/user"); 
const router = express.Router();


 //GET Profile - Fetch existing user data

router.get("/:id", async (req, res) => {
    try {
        // Validate User ID
        if (!mongoose.Types.ObjectId.isValid(req.params.id)) {
            return res.status(400).json({ message: "Invalid user ID" });
        }

        const user = await User.findById(req.params.id);
        if (!user) {
            return res.status(404).json({ message: "User not found" });
        }

        res.status(200).json(user);
    } catch (error) {
        res.status(500).json({ message: "Error fetching profile", error: error.message });
    }
});


 //PUT Profile - Update only necessary fields
 
router.put("/update/:id", async (req, res) => {
    try {
        const { name, email, dob, userRole, mobileNumber, nic, profilePicture } = req.body;

        // Validate User ID
        if (!mongoose.Types.ObjectId.isValid(req.params.id)) {
            return res.status(400).json({ message: "Invalid user ID" });
        }

        let user = await User.findById(req.params.id);
        if (!user) {
            return res.status(404).json({ message: "User not found" });
        }

        // Validation Rules
        if (mobileNumber && mobileNumber.length !== 10) {
            return res.status(400).json({ message: "Mobile number must be 10 digits." });
        }

        if (nic && nic.length !== 12) {
            return res.status(400).json({ message: "NIC must be 12 digits." });
        }

        // Update User Data (Only Fields Provided)
        user.name = name || user.name;
        user.email = email || user.email;
        user.dob = dob || user.dob;
        user.userRole = userRole || user.userRole;
        user.mobileNumber = mobileNumber || user.mobileNumber;
        user.nic = nic || user.nic;
        user.profilePicture = profilePicture || user.profilePicture;

        await user.save();

        res.status(200).json({ message: "Profile updated successfully", user });
    } catch (error) {
        res.status(500).json({ message: "Error updating profile", error: error.message });
    }
});

module.exports = router;
