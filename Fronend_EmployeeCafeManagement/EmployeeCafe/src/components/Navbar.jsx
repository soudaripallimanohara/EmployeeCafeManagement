import { Link, NavLink } from "react-router-dom";
import "./header.css";
import logo from "../assets/cafe.png";

export default function Navbar() {
  return (
    <div className="header">
      <div>
        <Link className="link">
          {/* <h2>Logo</h2> */}
          <img src={logo} alt="Logo" />
        </Link>
      </div>
      {/* <NavLink to={"/"}>Cafe</NavLink>
      <NavLink to={"employees"}>Employee</NavLink> */}
      <div>
        <ul>
          <li>
            <Link className="link" to="/">
              Cafe
            </Link>
          </li>
          <li>
            <Link className="link" to="employees">
              Employees
            </Link>
          </li>
        </ul>
      </div>
    </div>
  );
}
