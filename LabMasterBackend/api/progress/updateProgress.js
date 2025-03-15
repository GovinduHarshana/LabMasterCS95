const express = require("express");
const router = express.Router();
const Progress = require("../../models/progress");

router.post("/updateProgress", async (req, res) => {
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