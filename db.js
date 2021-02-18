import mariadb from 'mariadb'

const pool = mariadb.createPool({
  host: 'localhost',
  user: 'garasadae',
  database: 'garasadae',
  password: 'garasadae',
  connectionLimit: 5
});

export default pool;