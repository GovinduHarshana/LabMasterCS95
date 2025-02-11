<?php
include("db.php");

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $username = $_POST["username"];
    $password = $_POST["password"];

    // Retrieve the hashed password from the database
    $stmt = $conn->prepare("SELECT password FROM users WHERE username = ?");
    $stmt->bind_param("s", $username);
    $stmt->execute();
    $stmt->bind_result($hashedPassword);
    $stmt->fetch();
    $stmt->close();

    // Verify password using password_verify()
    if ($hashedPassword && password_verify($password, $hashedPassword)) {
        echo "Login successful!";
    } else {
        echo "Invalid username or password!";
    }
}

$conn->close();
?>