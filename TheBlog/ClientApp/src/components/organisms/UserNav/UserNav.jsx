import { useEffect, useState } from 'react';
import { NavItem } from 'reactstrap';
import MenuButton from '../../atoms/MenuButton/MenuButton';
import authService from '../../api-authorization/AuthorizeService';
import ActionButton from '../../atoms/ActionButton/ActionButton';
import { NavLink } from 'react-router-dom';
import { Link } from 'react-router-dom';

export default function UserNav() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);

  const getUser = async () => {
    const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()]);
    setUser(user);
    setIsAuthenticated(isAuthenticated);
  };
  useEffect(() => {
    getUser();
  }, []);

  const returnView = () => {
    if (isAuthenticated) {
      return (
        <>
          <NavItem className="m-1">
            <NavLink tag={Link} to="/account/profile">
              <MenuButton text={user?.name} />
            </NavLink>
          </NavItem>
          <NavItem className="m-1">
            <NavLink tag={Link} to="/manage/userroles">
              <MenuButton text="Manage Users" />
            </NavLink>
          </NavItem>
          <NavItem className="m-1">
            <NavLink tag={Link} className="text-dark mx-3" to={"/account/logout"}>Log out</NavLink>
          </NavItem>
        </>
      );
    }
    return (
      <NavItem>
        <Link to="/account/login">
          <ActionButton text="Log in" />
        </Link>
      </NavItem>
    );
  };

  return (
    <>
      { returnView() }
    </>
  );
}
