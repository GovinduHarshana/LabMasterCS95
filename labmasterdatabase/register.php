<?php
include("db.php");  

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Retrieve form data from Unity
    $username = $_POST["username"];
    $password = $_POST["password"];  
    $re_enterPassword = $_POST["re_enterPassword"];  
    $name = $_POST["name"];
    $email = $_POST["email"];
    $dob = $_POST["dob"];  
    $userRole = $_POST["userRole"];

    // Validate if passwords match
    if ($password !== $re_enterPassword) {
        echo "Passwords do not match!";
        exit();
    }

    // Hash the password for security before storing in database
    $hashedPassword = password_hash($password, PASSWORD_BCRYPT);

    // Check if the username is already taken
    $stmt = $conn->prepare("SELECT COUNT(*) FROM users WHERE username = ?");
    $stmt->bind_param("s", $username);
    $stmt->execute();
    $stmt->bind_result($count);
    $stmt->fetch();
    $stmt->close();

    if ($count > 0) {
        echo "Username already exists!";
    } else {
        // Insert new user data into the database
        $stmt = $conn->prepare("INSERT INTO users (username, password, name, email, dob, userRole) VALUES (?, ?, ?, ?, ?, ?)");
        $stmt->bind_param("ssssss", $username, $hashedPassword, $name, $email, $dob, $userRole);

        if ($stmt->execute()) {
            echo "User registered successfully!";
        } else {
            echo "Error: " . $stmt->error;
        }

        $stmt->close();
    }
}

$conn->close();
?>
