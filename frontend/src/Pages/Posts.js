import React from "react";
import "../App.css";
import SwaggerClient from 'swagger-client';
import { withCookies } from "react-cookie";
import { withRouter } from "react-router-dom";

class Posts extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            id: props.match.params.id,
            items: [],
            text: null,
            cookies: props.cookies.cookies,
            history: props.history,
            images: [],
            commentText: ""
        };

        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Post.get_Post__id_({ id: this.state.id }, {}),
                reason => console.error('failed to load the spec: ' + reason)
            )
            .then(
                result => this.setState({ text: <p><a href={`${result.body.link}`}>{result.body.link}</a><br/>{result.body.text}</p> }),
                reason => console.error('failed on api call: ' + reason)
            )
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Post.get_Post__id__comment({ id: this.state.id }, {}),
                reason => console.error('failed to load the spec: ' + reason)
            )
            .then(
                result => this.setState({ items: result.body.map((x) => <p>{x.message}</p>) }),
                reason => console.error('failed on api call: ' + reason)
            )
        
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Post.get_Post__id__image({ id: this.state.id }),
                reason => console.error("failed to load the spec: " + reason)
            )
            .then(
                result => this.setState({ images: result.body.map((x) => <img src={`http://localhost/Image/${x.id}`}></img>) }),
                reason => console.error('failed on api call: ' + reason)
            )
        
    }    
        
    handleSubmit(event) {    
        event.preventDefault();
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Post.post_Post__id__comment({id: this.state.id}, {requestBody: {postid: 0, message: this.state.commentText}}),
                reason => console.error('failed to load the spec: ' + reason)
            )
            .then(
                result => {this.state.history.go(0)},
                reason => console.error('failed on api call: ' + reason)
            )
    }

    render() {
        return (
            <div>
                <div className="outerContainer">
                    <div className="mainContent">
                        <div className="postcomments">
                            {this.state.images}
                            {this.state.text}
                        </div>
                        <div className="comments">
                            {this.state.items}
                        </div>
                        <form onSubmit={this.handleSubmit} className="writeComment">
                            <textarea
                                value={this.state.commentText}
                                onChange={(e) => this.setState({commentText: e.target.value})}
                                className="commentArea"></textarea>
                            <input type="submit" className="commentSubmit"></input>
                        </form>
                    </div>
                </div>
            </div>
        )
    }
}

export default withRouter(withCookies(Posts));
