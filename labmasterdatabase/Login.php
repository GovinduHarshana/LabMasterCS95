<?php
include ("db.php");

// Variables submitted by user
$loginUser = $_POST["username"];
$loginPass = $_POST["password"];


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