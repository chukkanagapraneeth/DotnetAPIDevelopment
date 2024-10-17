using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts()
        {
            string sqlPostQuery =
                @$"SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] FROM TutorialAppSchema.Posts";

            IEnumerable<Post> posts = _dapper.LoadData<Post>(sqlPostQuery);

            return posts;
        }

        [HttpGet("SinglePost/{PostId}")]
        public Post GetSinglePosts(int PostId)
        {
            string sqlSinglePostQuery =
                @$"SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] FROM TutorialAppSchema.Posts WHERE PostId = '{PostId}'";

            return _dapper.LoadDataSingle<Post>(sqlSinglePostQuery);
        }

        [HttpGet("PostsByUser/{UserID}")]
        public IEnumerable<Post> GetPostsByUser(int UserID)
        {
            string sql =
                @$"SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] FROM TutorialAppSchema.Posts WHERE UserID = '{UserID}'";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql =
                @$"SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] FROM TutorialAppSchema.Posts WHERE UserID = '{User.FindFirst("userId")?.Value}'";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpPost("CreatePost")]
        public IActionResult CreatePost(PostToAddDto postToAdd)
        {
            string sql =
                @$"
            INSERT INTO TutorialAppSchema.Posts (
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated]
            ) VALUES (
                '{User.FindFirst("userId")?.Value}',
                '{postToAdd.PostTitle}',
                '{postToAdd.PostContent}',
                GETDATE(),
                GETDATE()
            )";

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok("Post Created Successfully.");
            }

            throw new Exception(
                "Something Went Wrong, Unable to create the post. Please Try again."
            );
        }

        [HttpPost("EditPost")]
        public IActionResult EditPost(PostToEditDto postToEdit)
        {
            string sql =
                @$"UPDATE TutorialAppSchema.Posts SET 
                [PostTitle] = '{postToEdit.PostTitle}',
                [PostContent] = '{postToEdit.PostContent}', 
                [PostUpdated] = GETDATE() 
                    WHERE PostId = '{postToEdit.PostId}'
                    AND  UserID = '{User.FindFirst("userId")?.Value}'";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok("Post Updated Successfully.");
            }

            throw new Exception(
                "Something Went Wrong, Unable to Update the post. Please Try again."
            );
        }

        [HttpDelete("Post/{PostId}")]
        public IActionResult DeletePost(int PostId)
        {
            string sql =
                @$"
                DELETE FROM TutorialAppSchema.Posts WHERE PostId = '{PostId}' AND UserId = '{User.FindFirst("userId")?.Value}'
            ";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok("Post Deleted Successfully");
            }

            throw new Exception($"Could not delete post {PostId}");
        }

        [HttpGet("PostBySearch/{SearchParam}")]
        public IEnumerable<Post> PostBySearch(string SearchParam)
        {
            string sql =
                @$"
                SELECT [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] FROM TutorialAppSchema.Posts WHERE PostTitle LIKE '%{SearchParam}%' OR PostContent LIKE '%{SearchParam}%'                
                ";

            return _dapper.LoadData<Post>(sql);
        }
    }
}
