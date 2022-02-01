import React, { useState } from "react";
import "../App.css";
import SwaggerClient from 'swagger-client';
import { useCookies } from "react-cookie";
import { useHistory } from "react-router-dom";
import { Buffer } from 'buffer';

function Posts(props) {

    let history = useHistory();
    const [items, setItems] = useState("");
    const [link, setLink] = useState("");
    const [text, setText] = useState("");
    const [cookies, setCookie] = useCookies(["user"]);
    const [images, setImages] = useState("");
    const [commentText, setCommentText] = useState("");

    window.Buffer = Buffer;

    new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: cookies.Name, password: cookies.Password } } })
        .then(
            client => client.apis.Post.get_Post__id_({ id: props.match.params.id }, {}),
            reason => console.error('failed to load the spec: ' + reason)
        )
        .then(
            result => {
                setLink(result.body.link);
                setText(result.body.text);
            },
            reason => console.error('failed on api call: ' + reason)
        )
    new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: cookies.Name, password: cookies.Password } } })
        .then(
            client => client.apis.Post.get_Post__id__comment({ id: props.match.params.id }, {}),
            reason => console.error('failed to load the spec: ' + reason)
        )
        .then(
            result => setItems(result.body.map((x) => <p>{x.message}</p>)),
            reason => console.error('failed on api call: ' + reason)
        )

    new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: cookies.Name, password: cookies.Password } } })
        .then(
            client => client.apis.Post.get_Post__id__image({ id: props.match.params.id }),
            reason => console.error("failed to load the spec: " + reason)
        )
        .then(
            result => setImages(result.body.map((x) => <img src={`http://localhost/Image/${x.id}`}></img>),
            reason => console.error('failed on api call: ' + reason))
        )

    const handleSubmit = (event) => {    
        event.preventDefault();
        console.log("WUPPIEEEEE");
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: cookies.Name, password: cookies.Password } } })
        .then(
            client => client.apis.Post.post_Post__id__comment({id: props.match.params.id}, {requestBody: {postid: 0, message: commentText}}),
            reason => console.error('failed to load the spec: ' + reason)
        )
        .then(
            result => {},
            reason => console.error('failed on api call: ' + reason)
        )
    }    


    return (
        <div>
            <div className="outerContainer">
                <div className="mainContent">
                    <div className="postcomments">
                        {images}<br></br><a href={`${link}`}>{link}</a><br></br>
                        {text} <br></br>
                    </div>
                    <div className="comments">
                        {items}
                    </div>
                    <form onSubmit={handleSubmit} className="writeComment">
                        <textarea
                        value={commentText}
                        onChange={(e) => setCommentText(e.target.value)}
                        className="commentArea"></textarea>
                        <input type="submit" className="commentSubmit"></input>
                    </form>
                </div>
            </div>
        </div>
    )
}

export default Posts;