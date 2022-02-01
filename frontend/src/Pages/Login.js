import React from "react";
import "../App.css";
import Logo from "../img/Yomu_Logo.png";
import { useHistory } from "react-router-dom";
import { useState } from "react";
import SwaggerClient from 'swagger-client';
import { useCookies } from "react-cookie";

function Login() {
    let history = useHistory();

    const [name, setName] = useState("");
    const [password, setPassword] = useState("");
    const [cookies, setCookie] = useCookies(["user"]);

    const handle = () => {
        setCookie("Name", name, { path: "/" });
        setCookie("Password", password, { path: "/" });

        history.push("/home");
    };

    return (
        <div className="Login-form">
            <img src={Logo} alt="Yomu logo" className="logo"></img><br></br>
            <h2>Login</h2>
            <div className="Login-forminput">
                <form>
                    <input value={name} onChange={(e) => setName(e.target.value)}
                        type="text" placeholder="Benutzername" className="username" /> <br></br>
                    <input value={password} onChange={(e) => setPassword(e.target.value)}
                        type="password" placeholder="Passwort" className="password" /><br></br>
                    <input onClick={handle}
                        type="submit" className="Login-formbutton" /><br></br>
                    Noch kein Account?<button className="Login-formbutton"
                        onClick={() => {
                            history.push("/Register")
                        }}>
                        Registrieren</button>
                </form>
            </div>
        </div>
    )
}

export default Login;