using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly DataContext context;

        public PostsController(DataContext context) 
        {
            this.context = context;
        }

        //Get API/posts
        [HttpGet(Name = "GetPosts")]

        public ActionResult<List<Post>> Get()
        {
            return this.context.Posts.ToList();
        }

        [HttpGet("{id}", Name = "GetById")]

        public ActionResult<Post> GetById(Guid id){
            var post = this.context.Posts.Find(id);
            if (post is null){
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost(Name = "Create")]

        public ActionResult<Post> Create([FromBody]Post request)
        {
            var post = new Post
            {
                Id = request.Id,
                Title = request.Title,
                Body = request.Body,
                Date = request.Date
            };

            context.Posts.Add(post);
            var success = context.SaveChanges() > 0;

            if (success)
            {
                return Ok(post);
            }

            throw new Exception("Error creating post");
        }

        [HttpPut(Name = "Update")]

        public ActionResult<Post> Update ([FromBody] Post request)
        {
            var post = context.Posts.Find(request.Id);
            if (post == null)
            {
                throw new Exception("Could not find post");
            }
            
            post.Title = request.Title !=null ? request.Title : post.Title;
            post.Body = request.Body !=null ? request.Body : post.Body;
            post.Date = request.Date !=null ? request.Date : post.Date;

            var success = context.SaveChanges() > 0;

            if(success){
                return Ok(post);
            }

            throw new Exception("Error updating post");
        }
    }
}