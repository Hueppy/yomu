import React from "react";
import "../App.css";
import SwaggerClient from 'swagger-client';
import { withCookies } from "react-cookie";
import { withRouter } from "react-router-dom";

class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            items: [],
            cookies: props.cookies.cookies,
            history: props.history
        }
    }

    componentDidMount() {
        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: this.state.cookies.Name, password: this.state.cookies.Password } } })
            .then(
                client => client.apis.Home.get_Home({}, {}),
                reason => console.error('failed to load the spec: ' + reason)
            )
            .then(
                result => this.setState({items: result.body.map((x) => <p><a href={`/post/${x.id}`}>{x.text}</a></p>)}),
                reason => console.error('failed on api call: ' + reason)
            )
    }

    render() {
        return (
            <div>
                <div className="outerContainer">
                    <div className="mainContent">
                        <h1>Posts</h1>
                        <div className="posts">
                            {this.state.items}
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default withRouter(withCookies(Home));
