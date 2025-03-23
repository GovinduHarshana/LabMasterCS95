const express = require("express");
const router = express.Router();
const Note = require("../../models/note");
const mongoose = require("mongoose");

router.post("/createNote", async (req, res) => {
    try {
        console.log("Request body:", req.body); // Log the incoming request
        
        const { userId, title, content } = req.body;

        // Input Validation
        if (!userId || !title || !content) {
            return res.status(400).json({ message: "User ID, title, and content are required" });
        }

        if (!mongoose.Types.ObjectId.isValid(userId)) {
            return res.status(400).json({ message: "Invalid User ID" });
        }

        if (title.length > 255) {
            return res.status(400).json({ message: "Title is too long" });
        }

        // Sanitize content (basic example)
        const sanitizedContent = content.replace(/<[^>]*>/g, ''); // Remove HTML tags

        const newNote = new Note({
            userId,
            title,
            content: sanitizedContent, // Use sanitized content
        });
        await newNote.save();

        res.status(201).json({ message: "Note created successfully", note: newNote });
    } catch (error) {
        console.error("Error creating note:", error);

        if (error.name === "ValidationError") {
            return res.status(400).json({ message: "Validation error", errors: error.errors });
        }

        if (error.name === "MongoError" && error.code === 11000) {
            // Duplicate key error (e.g., if you had a unique index)
            return res.status(409).json({ message: "Duplicate key error" });
        }

        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;