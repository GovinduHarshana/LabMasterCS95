require("dotenv").config();
const express = require("express");
const cors = require("cors");
const mongoose = require("./config/db"); // Import MongoDB connection
const path = require("path");

const BACKEND_URL = process.env.BACKEND_URL || "http://localhost:5000";

const app = express();
app.use(express.json());
app.use(cors());

// Middleware to Catch Invalid JSON
app.use((err, req, res, next) => {
    if (err instanceof SyntaxError) {
        return res.status(400).json({ message: "Invalid JSON format" });
    }
    next();
});

// Import and use routes
// Auth routes
const loginRoute = require("./api/auth/login");
app.use("/api/auth", loginRoute);

const signupRoute = require("./api/auth/signup");
app.use("/api/auth", signupRoute);

const forgotPasswordRoute = require("./api/auth/forgot-password");
app.use("/api/auth", forgotPasswordRoute);

const resetPasswordRoute = require("./api/auth/reset-password");
app.use("/api/auth", resetPasswordRoute);

const ProfileRoute = require("./api/auth/profile");
app.use("/api/auth/profile", ProfileRoute);

// Note routes
const createNoteRoute = require("./api/note/createNote");
app.use("/api/note", createNoteRoute);

const retrieveNoteRoute = require("./api/note/retrieveNote");
app.use("/api/note", retrieveNoteRoute);

// Progress routes
const getProgressRoute = require("./api/progress/getProgress");
app.use("/api/progress", getProgressRoute);

const updateProgressRoute = require("./api/progress/updateProgress");
app.use("/api/progress", updateProgressRoute);



// For local development
if (process.env.NODE_ENV !== 'production') {
    const PORT = process.env.PORT || 5000;
    app.listen(PORT, () => {
      console.log(`✅ Server running on port ${PORT}`);
    });
  }
  
  // Export the app for Vercel
  module.exports = app;
