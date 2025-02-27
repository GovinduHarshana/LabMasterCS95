require("dotenv").config();
const express = require("express");
const mongoose = require("mongoose");
const cors = require("cors");
const bcrypt = require("bcryptjs");
const jwt = require("jsonwebtoken");


const app = express();
app.use(express.json());
app.use(cors());

// Set up the port and MongoDB URI from environment variables
const PORT = process.env.PORT || 5000;
const MONGO_URI = process.env.MONGO_URI || "mongodb+srv://labmasterAdmin:LabmasterCS95@labmastercluster.urqii.mongodb.net/LabMaster";

// Connect to MongoDB with improved error handling
mongoose.connect(MONGO_URI, {})
  .then(() => console.log("MongoDB connected"))
  .catch((err) => {
    console.error("MongoDB connection error:", err);
    process.exit(1); // Terminate the process if MongoDB connection fails
  });

// User Schema
const UserSchema = new mongoose.Schema({
    email: { type: String, required: true, unique: true },
    name: { type: String, required: true },
    dob: { type: String, required: true },
    userRole: { type: String, required: true, enum: ["Student", "Teacher", "Institute", "Other"] },
    passwordHash: { type: String, required: true },
    createdAt: { type: Date, default: Date.now }
});

const User = mongoose.model("User", UserSchema);

// Signup API
app.post("/signup", async (req, res) => {
    try {
        const { email, name, dob, userRole, password, rePassword } = req.body;

        // Validate input fields
        if (!email || !name || !dob || !userRole || !password || !rePassword) {
            return res.status(400).json({ message: "All fields are required" });
        }

        // Check if passwords match
        if (password !== rePassword) {
            return res.status(400).json({ message: "Passwords do not match" });
        }

        // Check if user already exists
        const existingUser = await User.findOne({ email });
        if (existingUser) {
            return res.status(400).json({ message: "User already exists" });
        }

        // Hash the password before saving
        const hashedPassword = await bcrypt.hash(password, 10);
        const newUser = new User({ email, name, dob, userRole, passwordHash: hashedPassword });

        await newUser.save();
        res.status(201).json({ message: "User registered successfully" });

    } catch (error) {
        console.error(error);  // Log the error for debugging
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});

// Login API
app.post("/login", async (req, res) => {
    try {
        const { email, password } = req.body;

        // Check if user exists
        const user = await User.findOne({ email });
        if (!user) {
            return res.status(400).json({ message: "User not found" });
        }

        // Compare provided password with stored hashed password
        const isMatch = await bcrypt.compare(password, user.passwordHash);
        if (!isMatch) {
            return res.status(400).json({ message: "Incorrect password" });
        }
        
        res.status(200).json({ message: "Login successful" });

    } catch (error) {
        res.status(500).json({ message: "Internal server error", error: error.message });
    }
});



// Middleware to catch invalid JSON
app.use((err, req, res, next) => {
    if (err instanceof SyntaxError) {
        return res.status(400).json({ message: "Invalid JSON" });
    }
    next();
});

// Start the server
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});


const express = require("express");
const mongoose = require("mongoose");
const cors = require("cors");


app.use(express.json());
app.use(cors());

mongoose.connect("your-mongodb-connection-string", {
    useNewUrlParser: true,
    useUnifiedTopology: true
});

const quizSchema = new mongoose.Schema({
    studentId: String,
    quizId: String,
    answers: [{ questionId: String, selectedAnswer: String }],
    submittedAt: { type: Date, default: Date.now }
});

const QuizAnswer = mongoose.model("QuizAnswer", quizSchema);

app.post("/save-answers", async (req, res) => {
    try {
        const newQuizAnswer = new QuizAnswer(req.body);
        await newQuizAnswer.save();
        res.status(201).send("Answers saved successfully!");
    } catch (error) {
        res.status(500).send(error.message);
    }
});

app.listen(5000, () => console.log("Server running on port 5000"));

