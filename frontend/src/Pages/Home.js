import React, { useState } from "react";
import "../App.css";
import SwaggerClient from 'swagger-client';
import { useCookies } from "react-cookie";
import { useHistory } from "react-router-dom";
import { Buffer } from 'buffer';

function Home() {
    let history = useHistory();
    const [items, setItems] = useState("");
    const [cookies, setCookie] = useCookies(["user"]);

    window.Buffer = Buffer;

    new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: cookies.Name, password: cookies.Password } } })
        .then(
            client => client.apis.Home.get_Home({}, {}),
            reason => console.error('failed to load the spec: ' + reason)
        )
        .then(
            result => setItems(result.body.map((x) => <p><a href={`/post/${x.id}`}>{x.text}</a></p>)),
            reason => console.error('failed on api call: ' + reason)
        )

    return (
        <div>
            <div className="outerContainer">
                <div className="mainContent">
                    <h1>Posts</h1>
                    <div className="posts">
                        {items}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Home;