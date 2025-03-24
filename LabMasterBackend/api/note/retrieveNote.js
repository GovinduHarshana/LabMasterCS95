const express = require("express");
const router = express.Router();
const Note = require("../../models/note");
const mongoose = require("mongoose");

router.post("/retrieveNote", async (req, res) => {
    try {
        const { userId } = req.body;

        if (!userId) {
            return res.status(400).json({ message: "User ID is required" });
        }

        if (!mongoose.Types.ObjectId.isValid(userId)) {
            return res.status(400).json({ message: "Invalid User ID" });
        }

        // Authentication/Authorization (Example - replace with your actual logic)
        // if(req.user.id !== userId){return res.status(403).json({message: "Unauthorized"})};

        const notes = await Note.find({ userId });

        res.status(200).json({ notes });
    } catch (error) {
        console.error("Error retrieving notes:", error);

        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

module.exports = router;