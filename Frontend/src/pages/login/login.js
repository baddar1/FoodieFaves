import "./Login.css";
import googleIcon from "../../asset/googleIcon.png";
import foodImg from "../../asset/foodImg.png";
import { useState } from "react";
export default function Login() {
  const [formInputs, setFormInput] = useState({
    email: "",
    passworde: "",
  });
  const [error, setError] = useState("");

  return (
    <div className="main-grid">
      <div>
        <form
          className="left-grid"
          onSubmit={(event) => {
            event.preventDefault();
            console.log(formInputs);
          }}
        >
          <div className="welcome">
            <p className="welcome">welcome back</p>
            <p className="please">please enter your information</p>
          </div>
          <div className="Email">
            <label className="labels">Email</label>
            <br />
            <input
              className="emailbox"
              type="email"
              placeholder="Enter your email"
              required
              value={formInputs.email}
              onChange={(event) => {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(event.target.value)) {
                  setError("Please enter a valid email address.");
                } else {
                  setError("");
                }
                setFormInput({ ...formInputs, email: event.target.value });
              }}
            />
            {error && <p style={{ color: "red" }}>{error}</p>}
          </div>
          <div className="password">
            <label className="labels">password</label>
            <br />
            <input
              className="password-box"
              type="password"
              placeholder="********"
              required
              value={formInputs.passworde}
              onChange={(event) => {
                setFormInput({ ...formInputs, passworde: event.target.value });
              }}
            />
          </div>
          <div className="options">
            <div>
              <label>
                <input type="checkbox" /> Remember me
              </label>
            </div>
            <div>
              <a className="forgot-password" href="#">
                forgot password
              </a>
            </div>
          </div>
          <div>
            <button className="sign-in" type="submit">
              Sign in
            </button>
            <br />
          </div>
          <div>
            <button className="google-sign-in" type="button">
              <img className="google-icon" src={googleIcon} alt="Google icon" />{" "}
              sign in with google
            </button>
            <br />
          </div>
          <div className="create">
            <label>
              Don't have an account! <a href="#">sign up for free!</a>
            </label>
          </div>
        </form>
      </div>
      <div>
        <img className="picture" src={foodImg} alt="Decoration" />
      </div>
    </div>
  );
}
