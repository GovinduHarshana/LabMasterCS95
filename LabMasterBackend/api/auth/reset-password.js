const express = require("express");
const router = express.Router();
const bcrypt = require("bcryptjs");
const User = require("../../models/user");

router.post("/reset-password", async (req, res) => {
    try {
        const { token, newPassword } = req.body;
        if (!isValidPassword(newPassword)) {
            return res.status(400).json({ message: "Weak password: Must be 8+ characters with 1 uppercase, 1 number, and 1 special character." });
        }

        // Using the correct 'User' model here
        const user = await User.findOne({
            resetToken: token,
            resetTokenExpiry: { $gt: Date.now() } // Check token is not expired
        });

        if (!user) return res.status(400).json({ message: "Invalid or expired token" });

        const hashedPassword = await bcrypt.hash(newPassword, 10);
        await User.updateOne(
            { resetToken: token },
            { $set: { passwordHash: hashedPassword, resetToken: null, resetTokenExpiry: null } }
        );

        res.json({ message: "Password has been reset successfully" });
    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;
