const express = require("express");
const router = express.Router();
const Note = require("../../models/note");

router.post("/createNote", async (req, res) => {
    try {
        const { userId, title, content } = req.body;

        if (!userId || !title || !content) {
            return res.status(400).json({ message: "User ID, title, and content are required" });
        }

        const newNote = new Note({ userId, title, content });
        await newNote.save();

        res.status(201).json({ message: "Note created successfully", note: newNote });
    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;
