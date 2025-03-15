const express = require("express");
const router = express.Router();
const crypto = require("crypto");
const nodemailer = require("nodemailer");
const User = require("../../models/user");

const transporter = nodemailer.createTransport({
    service: "gmail",
    secure: true,
    port: 465,
    auth: {
        user: process.env.EMAIL_USER,
        pass: process.env.EMAIL_PASS
    }
});

router.post("/forgot-password", async (req, res) => {
    try {
        const { email } = req.body;
        if (!email) return res.status(400).json({ message: "Email is required." });
        
        const user = await User.findOne({ email });  // Using the User model here
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

module.exports = router;
