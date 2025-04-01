<?php
    // Load environment variables
    $envFile = __DIR__ . '/.env';
    if (file_exists($envFile)) {
        $lines = file($envFile, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
        foreach ($lines as $line) {
            if (strpos($line, '=') !== false && strpos($line, '#') !== 0) {
                list($key, $value) = explode('=', $line, 2);
                $_ENV[trim($key)] = trim($value);
            }
        }
    }

    $con = mysqli_connect(
        $_ENV['DB_HOST'],
        $_ENV['DB_USER'],
        $_ENV['DB_PASS'],
        $_ENV['DB_NAME']
    );

    if (mysqli_connect_error()) {
        echo "Failed to connect to MySQL: " . mysqli_connect_error();
        exit();
    }

    $score = $_POST['score'];
    $time = $_POST['time'];

    $sql = "INSERT INTO runs (score, time) VALUES ('$score', '$time')";
    mysqli_query($con, $sql) or die("Error: " . mysqli_error($con));

    echo "0";
    mysqli_close($con);
?>