<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "labmasterdatabase";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}
echo "Connected successfully, now we will show the users.<br><br>";

$sql = "SELECT userId, username, firstName, lastName, email FROM users";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // Output data of each row
    while ($row = $result->fetch_assoc()) {
        echo " - UserId: " . $row["userId"] . " " . " - Username: " . $row["username"] . " " . " - Name: " . $row["firstName"] . " " . $row["lastName"] . " " . " - Email: " . $row["email"] . "<br>";
    }
} else {
    echo "0 results";
}

$conn->close();

?>