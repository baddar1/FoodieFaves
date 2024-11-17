import "./Signup.css";
import googleIcon from "../../asset/googleIcon.png";
import foodImg from "../../asset/foodImg.png";
import { useState } from "react";
function SignUpForm() {
  const [formInputs, setFormInput] = useState({
    email: "",
    Username: "",
    phonenumbers: "",
    password: "",
    confirmpassword: "",
  });
  const [erroremail, setErroremail] = useState("");
  const [errorpassword, setErrorpassword] = useState("");
  const [errorRetypepassword, setErrorRetypepassword] = useState("");

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
          <div className="first">
            <p className="create">Create account</p>
            <p className="please">Please enter your information</p>
          </div>

          <div className="details">
            <label className="details-label">Email</label>
            <br />
            <input
              className="text-box"
              type="email"
              placeholder="Enter your email"
              required
              value={formInputs.email}
              onChange={(event) => {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(event.target.value)) {
                  setErroremail("Please enter a valid email address.");
                } else {
                  setErroremail("");
                }
                setFormInput({ ...formInputs, email: event.target.value });
              }}
            />
            {erroremail && <p style={{ color: "red" }}>{erroremail}</p>}
          </div>

          <div className="details">
            <label className="details-label">User name</label>
            <br />
            <input
              className="text-box"
              type="text"
              placeholder="Enter your User name"
              required
              value={formInputs.Username}
              onChange={(event) => {
                setFormInput({ ...formInputs, Username: event.target.value });
              }}
            />
          </div>
          <div className="details">
            <label className="details-label">Phone numbers</label>
            <br />
            <input
              className="text-box"
              type="tel"
              placeholder="Enter your User name"
              required
              value={formInputs.phonenumbers}
              onChange={(event) => {
                setFormInput({
                  ...formInputs,
                  phonenumbers: event.target.value,
                });
              }}
            />
          </div>

          <div className="details">
            <label className="details-label">Password</label>
            <br />
            <input
              className="text-box"
              type="password"
              placeholder="Enter password"
              value={formInputs.password}
              onChange={(event) => {
                setFormInput({ ...formInputs, password: event.target.value });
                const minLength = 8;
                const hasNumber = /\d/;
                const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/;

                if (event.target.value.length < minLength) {
                  setErrorpassword(
                    "Password must be at least 8 characters long."
                  );
                } else if (!hasNumber.test(event.target.value)) {
                  setErrorpassword(
                    "Password must contain at least one number."
                  );
                } else if (!hasSpecialChar.test(event.target.value)) {
                  setErrorpassword(
                    "Password must contain at least one special character."
                  );
                } else {
                  setErrorpassword("");
                }
              }}
            />
            {errorpassword && <p style={{ color: "red" }}>{errorpassword}</p>}
          </div>

          <div className="details">
            <label className="details-label">Re-type password</label>
            <br />
            <input
              className="text-box"
              type="password"
              placeholder="Re-type password"
              value={formInputs.confirmpassword}
              onChange={(event) => {
                setFormInput({
                  ...formInputs,
                  confirmpassword: event.target.value,
                });
                if (event.target.value !== formInputs.password) {
                  setErrorRetypepassword("Passwords do not match.");
                } else {
                  setErrorRetypepassword("");
                }
              }}
            />
            {errorRetypepassword && (
              <p style={{ color: "red" }}>{errorRetypepassword}</p>
            )}
          </div>

          <div>
            <button className="sign-up" type="submit">
              Sign up
            </button>
            <br />
          </div>

          <div>
            <button className="google-sign-in" type="button">
              <img className="google-icon" src={googleIcon} alt="Google Icon" />
              Sign in with Google
            </button>
            <br />
          </div>
        </form>
      </div>

      <div>
        <img className="picture" src={foodImg} alt="Decorative" />
      </div>
    </div>
  );
}

export default SignUpForm;
