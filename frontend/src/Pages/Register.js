import React from "react";
import "../App.css";
import Logo from "../img/Yomu_Logo.png";
import { useHistory } from "react-router-dom";
import { useState } from "react";
import SwaggerClient from 'swagger-client';
import { Buffer } from 'buffer';

function Register() {
    let history = useHistory();
    window.Buffer = Buffer;

    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [repeatpassword, setRepeatpassword] = useState("");


    const handleSubmit = (event) => {
        event.preventDefault();
        if (password === repeatpassword) {
            new SwaggerClient('http://localhost/swagger/v1/swagger.json')
                .then(
                    client => client.apis.Login.post_Login({ email: email, password: password }),
                    reason => console.error('failed to load the spec: ' + reason)
                )
                .then(
                    result => {
                        new SwaggerClient({ url: 'http://localhost/swagger/v1/swagger.json', authorizations: { basic: { username: email, password: password } } })
                            .then(
                                client => client.apis.User.post_User({}, { requestBody: { id: username, email: email } }),
                                reason => console.error('failed to load the spec: ' + reason)
                            )
                            .then(
                                result => history.push("/Login"),
                                reason => console.error('failed on api call: ' + reason)
                            )
                    },
                    reason => console.error('failed on api call: ' + reason)
                );
        }
    }


    return (
        <div className="Login-form">
            <img src={Logo} alt="Yomu logo" className="logo"></img><br></br>
            <h2>Registrierung</h2>
            <div className="Login-forminput">
                <form onSubmit={handleSubmit}>
                    <input value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        type="text" placeholder="Benutzername" className="username" /><br></br>
                    <input value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        type="email" placeholder="Email" className="email" /><br></br>
                    <input value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        type="password" placeholder="Passwort" className="password" /><br></br>
                    <input value={repeatpassword}
                        onChange={(e) => setRepeatpassword(e.target.value)}
                        type="password" placeholder="Passwort wiederholen" className="repeatpassword" /><br></br>
                    <p >Passwörter stimmen nicht überein!</p>
                    <input type="submit" className="Login-formbutton" /><br></br>
                    Schon Mitglied?<button className="Login-formbutton"
                        onClick={() => {
                            history.push("/Login")
                        }}>Login</button>
                </form>
            </div>
        </div>
    )
}


export default Register;