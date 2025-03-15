const express = require("express");
const router = express.Router();
const Note = require("../../models/note");

router.post("/retrieveNote", async (req, res) => {
    try {
        const { userId } = req.query;

        if (!userId) {
            return res.status(400).json({ message: "User ID is required" });
        }

        const notes = await Note.find({ userId });

        res.status(200).json({ notes });
    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;
