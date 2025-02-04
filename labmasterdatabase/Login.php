<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "labmasterdatabase";

// Variables submitted by user
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

// Create connection
$conn = new mysqli($servername, $username, $password);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}
echo "Connected successfully, now we will show the users.<br><br>";

$sql = "SELECT password FROM users WHERE username = '$loginUser'";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // Output data of each row
  while($row = $result->fetch_assoc()) {
    if ($row["password"] == $loginUser) {
      echo "Login successful!";
    } else {
      echo "Wrong credentials.";
    }
  }
} else {
  echo "Username does not exist.";
}
 
$conn->close();

?>