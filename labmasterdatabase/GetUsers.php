<?php
include ("db.php");

$sql = "SELECT userId, username, name, email, userRole FROM users";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // Output data of each row
    while ($row = $result->fetch_assoc()) {
        echo " - UserId: " . $row["userId"] . " " . " - Username: " . $row["username"] . " " . " - Name: " . $row["name"] ." " . " - Email: " . $row["email"] .  " " . " - User Role: " .  $row["userRole"] . "<br>";
    }
} else {
    echo "0 results";
}

$conn->close();

?>