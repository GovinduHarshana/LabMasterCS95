const express = require("express");
const Progress = require("../../models/progress");
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

module.exports = router;