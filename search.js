import express from 'express';
import db from '../config/db.js';

const router = express.Router();

router.get('/', async (req, res) => {
  try {
    const category = req.query.category;
    const query = req.query.query;
    let limit = 999;
    if(req.query.limit !== undefined) {
      limit = parseInt(req.query.limit);
    }
    if(category === '글귀') {
      const data = await db.query('SELECT * FROM phrase WHERE phrase LIKE "%' + query + '%" LIMIT ?', [limit]);
      res.status(200).json(data);
    } else if (category === '태그') {
      const data = await db.query('SELECT * FROM phrase WHERE description LIKE "%' + query + '%" LIMIT ?', [limit]);
      res.status(200).json(data);
    } else if (category === '분야') {
      const data = await db.query('SELECT * FROM phrase WHERE categoryName = ? LIMIT ?', [query, limit]);
      res.status(200).json(data);
    } else {
      res.status(400).json({
        message: 'unexpected category'
      })
    }
  } catch (error) {
    res.status(500).json(error);
    console.log(error);
  }
});

export default router;