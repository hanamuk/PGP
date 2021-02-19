import express from 'express';
import db from '../config/db.js';
import crypto from 'crypto-promise';
import jwt from 'jsonwebtoken';

const router = express.Router();
const salt = 'swhackaton';
const jwtSecret = 'swhackaton'

async function pwHash(password) {
  const hash = await crypto.hash('sha256')(salt+password);
  return hash.toString('hex');
}

//사용자 회원가입
router.post('/register', async (req, res) => {
  const {id, password, age, sex} = req.body;
  try {
    const exist = await db.query('SELECT EXISTS(SELECT * FROM user WHERE ID = ?) as isExist', [id]);
    if(exist[0]['isExist']) {
      res.status(409).json({
        message: 'already exist user id'
      });
      return;
    }
    const hash = await pwHash(password);
    await db.query('INSERT INTO user(ID, Password, age, sex) VALUE(?, ?, ?, ?)', [id, hash, age, sex]);
    res.status(200).json({
      message: 'success'
    });
  } catch (error) {
    res.status(500).json(error);
  }
})

//로그인 (로그인 토큰 발급)
router.post('/login', async (req, res) => {
  const {id, password} = req.body;
  try {
    const exist = await db.query('SELECT EXISTS(SELECT * FROM user WHERE ID = ?) as isExist', [id]);
    if(!exist[0]['isExist']) {
      res.status(404).json({
        message: 'user id not found'
      });
      return;
    }
    let row = await db.query('SELECT * FROM user WHERE ID = ?', id);
    let data = row[0];
    if(data.Password !== await pwHash(password)) {
      res.status(409).json({
        message: 'password wrong!'
      });
      return;
    }
    const token = jwt.sign({
        id: id,
      },
      jwtSecret,
      {
        expiresIn: '1d',
        issuer: 'user'
      }
    );
    res.status(200).json({
      message: 'success',
      token: token
    });
  } catch (error) {
    res.status(500).json(error);
  }
})

export default router;
