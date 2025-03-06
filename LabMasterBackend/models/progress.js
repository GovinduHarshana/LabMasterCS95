const mongoose = require("mongoose");

const progressSchema = new mongoose.Schema({
  userId: { type: String, required: true },
  practicalsCompleted: { type: Number, default: 0 },
  totalPracticals: { type: Number, default: 10 },
  quizzesCompleted: { type: Number, default: 0 },
  totalQuizzes: { type: Number, default: 10 }
});

module.exports = mongoose.model("Progress", progressSchema);
