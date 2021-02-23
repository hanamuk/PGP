import express from 'express'
import verifyToken from '../middleware/verifyToken.js'
import db from '../config/db.js'
const router = express.Router()

//명언 작성
router.post('/', verifyToken, async (req, res) => {
    const writerId = res.locals.jwtPayload.id;
    const {phrase, categoryName, referenceName, description} = req.body;
    try {
      const exist = await db.query('SELECT EXISTS(SELECT * FROM phrase WHERE phrase = ?) as isExist', [phrase]);
      if(exist[0]['isExist']) {
        res.status(409).json({
          message: 'exist phrase'
        });
        return;
      }
      await db.query('INSERT INTO phrase(phrase, categoryName, writerId, referenceName, description) VALUE(?, ?, ?, ?, ?)', [phrase, categoryName, writerId, referenceName, description]);
      res.status(200).json({
        message: 'success'
      });
    } catch (error) {
      console.log(error);
      res.status(500).json(error);
    }
  })
  

//명언 불러오기
router.get('/:id', async (req, res) => {
  try {
    const data = await db.query('SELECT * FROM phrase WHERE id = ?', [req.params.id]);
    res.status(200).json(data[0]);
  } catch(error) {
    console.log(error);
    res.status(500).json(error);
  }
});


//유저 좋아요 추가
router.post('/:id/like', verifyToken, async (req, res) => {
  const userID = res.locals.jwtPayload.id;
  const phraseID = req.params.id;
  try {
    await db.query('INSERT INTO liked(userID, phraseID) VALUE(?, ?)', [userID, phraseID]);
    res.status(200).json({
      message: 'success'
    })
  } catch(error) {
    console.log(error);
    res.status(500).json(error);
  }
})

//유저 좋아요 삭제
router.delete('/:id/like', verifyToken, async (req, res) => {
  const userID = res.locals.jwtPayload.id;
  const phraseID = req.params.id;
  try {
    await db.query('DELETE FROM liked WHERE userID = ? AND phraseID = ?', [userID, phraseID]);
    res.status(200).json({
      message: 'success'
    })
  } catch(error) {
    console.log(error);
    res.status(500).json(error);
  }
})

//명언 댓글 가져오기
router.get('/:id/comment', async (req, res) => {
  const phraseID = req.params.id;
  try {
    const data = await db.query('SELECT * FROM comment WHERE phraseID = ?', [phraseID]);
    for(let i = 0;i < data.length; ++i) {
      data[i].like = (await db.query('SELECT count(*) as `like` FROM comment_like WHERE commentID = ?', [data[i].id]))[0]['like'];
    }
    data.sort((a, b) => {
      return b.like - a.like;
    })
    res.status(200).json(data);
  } catch(error) {
    console.log(error);
    res.status(500).json(error);
  }
});

//명언 댓글 작성
router.post('/:id/comment', verifyToken, async (req, res) => {
  const userID = res.locals.jwtPayload.id;
  const phraseID = req.params.id;
  const {comment} = req.body;
  try {
    const data = await db.query('INSERT INTO comment(userID, comment, phraseID) value(?, ?, ?)', [userID, comment, phraseID]);
    res.status(200).json({
      message: 'success'
    });
  } catch(error) {
    console.log(error);
    res.status(500).json(error);
  }
});

//명언 댓글 삭제
router.delete('/:id/comment/:comment_id', verifyToken, async (req, res) => {
  const userID = res.locals.jwtPayload.id;
  const phraseID = parseInt(req.params.id);
  const commentID = parseInt(req.params.comment_id);
  try {
    const check = await db.query('SELECT * FROM comment WHERE id = ?', [commentID]);
    if(check[0].userID !== userID) {
      res.status(409).json({
        message: 'different userID'
      })
      return;
    }
    if(check[0].phraseID !== phraseID) {
      res.status(409).json({
        message: 'different phraseID'
      })
      return;
    }
    await db.query('DELETE FROM comment WHERE id = ?', [commentID]);
    res.status(200).json({
      message: 'success'
    });
  } catch (error) {
    console.log(error);
    res.status(500).json(error);
  }
});

//명언 댓글 좋아요
router.post('/:id/comment/:comment_id', verifyToken, async (req, res) => {
  const phraseID = parseInt(req.params.id);
  const userID = res.locals.jwtPayload.id;
  const commentID = parseInt(req.params.comment_id);
  try {
    const check = await db.query('SELECT * FROM comment WHERE id = ?', [commentID]);
    if(check[0].phraseID !== phraseID) {
      res.status(409).json({
        message: 'different phraseID'
      })
      return;
    }
    await db.query('INSERT INTO comment_like(userID, commentID) VALUE(?, ?)', [userID, commentID]);
    res.status(200).json({
      message: 'success'
    })
  } catch (error) {
    console.log(error);
    res.status(500).json(error);
  }
})

export default router;