const mongoose = require("mongoose");

const MONGO_URI = process.env.MONGO_URI;
if (!MONGO_URI) {
    console.error("MongoDB URI is missing in .env file");
    process.exit(1);
}

mongoose.connect(MONGO_URI)
    .then(() => console.log("✅ MongoDB connected"))
    .catch((err) => {
        console.error("❌ MongoDB connection error:", err);
        process.exit(1);
    });

module.exports = mongoose;