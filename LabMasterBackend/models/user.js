const mongoose = require("mongoose");

const UserSchema = new mongoose.Schema({
    email: { type: String, required: true, unique: true },
    name: { type: String, required: true },
    dob: { type: String, required: true },
    userRole: { type: String, required: true, enum: ["Student", "Teacher", "Institute", "Other"] },
    mobileNumber: { type: String, default: "" },
    nic: { type: String, default: "" }, 
    profilePicture: { type: String, default: "" },
    passwordHash: { type: String, required: true },
    resetToken: { type: String, default: null },
    resetTokenExpiry: { type: Date, default: null },
    createdAt: { type: Date, default: Date.now }
});

module.exports = mongoose.model("User", UserSchema);