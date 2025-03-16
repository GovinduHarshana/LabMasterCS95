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

        const user = await User.findOne({ email });
        if (!user) return res.status(404).json({ message: "User not found." });

        const resetToken = crypto.randomBytes(20).toString("hex");
        user.resetToken = resetToken;
        user.resetTokenExpiry = Date.now() + 3600000;
        await user.save();

        // No referer check, always send token in email
        const mailOptions = {
            from: `LabMaster Support <${process.env.EMAIL_USER}>`,
            to: email,
            subject: "Reset Your Password - LabMaster",
            html: `
                <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                    <h2>LabMaster Password Reset</h2>
                    <p>Hello,</p>
                    <p>Please use the following token in your LabMaster app to reset your password:</p>
                    <p><strong>${resetToken}</strong></p>
                    <p>This token is valid for 1 hour.</p>
                    <p>If you did not request a password reset, please ignore this email.</p>
                    <p>Thank you,</p>
                    <p>The LabMaster Team</p>
                </div>
            `
        };

        await transporter.sendMail(mailOptions);
        res.json({ message: "Check your email for the reset token!" });
    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;