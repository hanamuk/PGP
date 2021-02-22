import express from 'express';
import db from '../config/db.js';

const router = express.Router();

//공명의 주머니
router.get('/', async (req, res) => {
  let limit = 999;
  let query = 'SELECT * FROM phrase';
  let param = []
  if(req.query.category !== undefined) {
    query += ' WHERE referenceName = ?'
    param.push(req.query.category);
  }
  query += ' ORDER BY RAND() LIMIT ?'
  param.push(limit);
  if(req.query.limit !== undefined) {
    limit = parseInt(req.query.limit);
  }
  try {
    const data = await db.query(query, param);
    res.status(200).json(data);
  } catch (error) {
    console.log(error);
    res.status(500).json(error);
  }
});

export default router;