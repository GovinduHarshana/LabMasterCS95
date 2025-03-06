const express = require("express");
const Progress = require("../models/progress");
const router = express.Router();

// Get user progress
router.get("/:userId", async (req, res) => {
  try {
    const progress = await Progress.findOne({ userId: req.params.userId });
    if (!progress) return res.status(404).json({ message: "User progress not found" });

    res.json(progress);
  } catch (error) {
    res.status(500).json({ message: "Error fetching progress", error });
  }
});

// Update user progress
router.post("/update", async (req, res) => {
  try {
    const { userId, practicalsCompleted, quizzesCompleted } = req.body;

    let progress = await Progress.findOne({ userId });

    if (!progress) {
      progress = new Progress({ userId, practicalsCompleted, quizzesCompleted });
    } else {
      progress.practicalsCompleted = practicalsCompleted;
      progress.quizzesCompleted = quizzesCompleted;
    }

    await progress.save();
    res.json({ message: "Progress updated", progress });
  } catch (error) {
    res.status(500).json({ message: "Error updating progress", error });
  }
});

module.exports = router;
