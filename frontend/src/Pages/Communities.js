import React from "react";
import "../App.css";
import SwaggerClient from 'swagger-client';
import { withCookies } from "react-cookie";
import { withRouter } from "react-router-dom";

class Communities extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            id: props.match.params.id,
            description: null,
            posts: [],
            cookies: props.cookies.cookies,
            history: props.history,
            commentText: "",
        }

        this.handleSubmit = this.handleSubmit.bind(this)
    }

    componentDidMount() {
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Community.get_Community__id_({id: this.state.id}, {}),
                reason => console.error('failed to load the spec: ' + reason)
            )
            .then(
                result => this.setState({ description: <p>{result.body.description}</p> }),
                reason => console.error('failed on api call: ' + reason)
            )
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Community.get_Community__id__post({id: this.state.id}, {}),
                reason => console.error('failed to load the spec: ' + reason)
            )
            .then(
                result => this.setState({ posts: result.body.map(x => <p><a href={`/post/${x.id}`}>{x.text}</a></p>) }),
                reason => console.error('failed on api call: ' + reason)
            )
    }

    handleSubmit(event) {
        event.preventDefault();
        
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Community.post_Community__id__post({id: this.state.id}, { requestBody: {id: 0, communityId: "", text: this.state.commentText}}),
                reason => console.error('failed to load the spec: ' + reason)
            )
            .then(
                result => this.state.history.go(0),
                reason => console.error('failed on api call: ' + reason)
            )

    }

    render() {
        console.log(this.state)
        return (
            <div className="mainContent">
                {this.state.description}
                <div className="posts">
                    {this.state.posts}
                </div>
                <form onSubmit={this.handleSubmit} className="writeComment">
                    <textarea
                        value={this.state.commentText}
                        onChange={(e) => this.setState({commentText: e.target.value})}
                        className="commentArea"></textarea>
                    <input type="submit" className="commentSubmit"></input>
                </form>
            </div>
        );
    }
}

export default withRouter(withCookies(Communities));
