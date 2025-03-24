const mongoose = require("mongoose");

const noteSchema = new mongoose.Schema({
    userId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true,
        index: true, // Add index for faster queries
    },
    title: {
        type: String,
        required: true,
        index: true, // Add index for faster queries
    },
    content: {
        type: String,
        required: true,
    },
    createdAt: {
        type: Date,
        default: Date.now,
    },
});

module.exports = mongoose.model("Note", noteSchema);