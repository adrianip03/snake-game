const express = require('express');
const mysql = require('mysql2');
const cors = require('cors');
const app = express();
require('dotenv').config();

// Middleware
app.use(cors());
app.use(express.json());

// MySQL connection
const connection = mysql.createConnection({
  host: process.env.DB_HOST,
  user: process.env.DB_USER, 
  password: process.env.DB_PASS,
  database: process.env.DB_NAME
});

// Connect to MySQL
connection.connect(error => {
  if (error) throw error;
  console.log('Successfully connected to the database.');
});

// Create table if not exists
const createTableQuery = `
  CREATE TABLE IF NOT EXISTS runs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    score INT NOT NULL,
    time_played INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
  )
`;

connection.query(createTableQuery, (error) => {
  if (error) throw error;
  console.log('Table created or already exists.');
});

// Save game run
app.post('/newRun', (req, res) => {
  const { score, time } = req.body;
  
  const query = 'INSERT INTO runs (score, time_played) VALUES (?, ?)';
  connection.query(query, [score, time], (error, results) => {
    if (error) {
      console.error('Error saving score:', error);
      res.status(500).json({ success: false, message: 'Error saving score' });
      return;
    }
    res.json({ success: true, message: 'Score saved successfully' });
  });
});

// Get all runs
app.get('/runs', (req, res) => {
  const query = 'SELECT id, score, time_played, created_at FROM runs';
  connection.query(query, (error, results) => {
    if (error) {
      console.error('Error fetching runs:', error);
      res.status(500).json({ success: false, message: 'Error fetching runs' });
      return;
    }
    res.json(results);
  });
});

// Get all runs and display in a table format
app.get('/runs_table', (req, res) => {
  const query = 'SELECT id, score, time_played, created_at FROM runs';

  connection.query(query, (error, results) => {
    if (error) {
      console.error('Error fetching runs:', error);
      res.status(500).send('Error fetching runs');
      return;
    }

    // Generate HTML table
    let tableHtml = `
      <!DOCTYPE html>
      <html lang="en">
      <head>
        <title>Runs Table</title>
        <style>
          table {
            width: 80%;
            margin: 20px auto;
            border-collapse: collapse;
          }
          th, td {
            padding: 10px;
            text-align: center;
            border: 1px solid #ddd;
          }
          th {
            background-color: #f4f4f4;
          }
        </style>
      </head>
      <body>
        <h1 style="text-align: center;">Runs Table</h1>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Score</th>
              <th>Time</th>
              <th>Created At</th>
            </tr>
          </thead>
          <tbody>
    `;

    // Add rows to the table
    results.forEach(row => {
      tableHtml += `
        <tr>
          <td>${row.id}</td>
          <td>${row.score}</td>
          <td>${row.time_played}</td>
          <td>${row.created_at}</td>
        </tr>
      `;
    });

    // Close the table
    tableHtml += `
          </tbody>
        </table>
      </body>
      </html>
    `;

    // Send the HTML response
    res.send(tableHtml);
  });
});


const PORT = 3000;
app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
}); 