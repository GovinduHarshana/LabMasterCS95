const express = require("express");
const User = require("../../models/user");
const router = express.Router();

// Update User Profile (using _id)
router.put("/update/:id", async (req, res) => {
    try {
        const { name, dob, mobileNumber, nic, profilePicture } = req.body;

        // Check if user exists
        let user = await User.findById(req.params.id); // Find user by _id
        if (!user) {
            return res.status(404).json({ message: "User not found" });
        }

        // Update user profile
        user.name = name || user.name;
        user.dob = dob || user.dob;
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