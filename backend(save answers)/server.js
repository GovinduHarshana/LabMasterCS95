const express = require("express");
const { MongoClient } = require("mongodb");
const cors = require("cors");

const app = express();
app.use(express.json());
app.use(cors()); // Allow requests from Unity

const uri = "mongodb+srv://labmasterAdmin:<LabmasterCS95>@labmastercluster.urqii.mongodb.net/?retryWrites=true&w=majority&appName=LabMasterCluster"; // Replace with your MongoDB Atlas URI
const client = new MongoClient(uri);

async function connectDB() {
    await client.connect();
    then(() => console.log("Connected to MongoDB"))
    .catch(err => console.error("MongoDB connection error:", err));
    return client.db("LabMasterDB"); // Use the same database name as in MongoDB Atlas
}

app.post("/save-quiz", async (req, res) => {
    try {
        const db = await connectDB();
        const quizCollection = db.collection("quiz_answers");

        const quizData = {
            studentId: req.body.studentId, // Unique student ID
            quizSection: req.body.quizSection, // Section 1 or 2
            answers: req.body.answers, // Array of answers
            timestamp: new Date()
        };

        const result = await quizCollection.insertOne(quizData);
        res.status(201).json({ message: "Quiz answers saved!", result });
    } catch (error) {
        res.status(500).json({ message: "Error saving data", error });
    }
});

app.listen(3000, () => console.log("Server running on port 3000"));
