import { useEffect, useState } from "react";
import { Input, Table } from "reactstrap";

export default function RoleManagerMenu() {
  const [roles, setRoles] = useState([]);
  const [userRoles, setUserRoles] = useState([]);

  const fetchRoles = async () => {
    await fetch('/api/role/getallroles')
    .then(async (data) => {
      if (data.ok) {
        const jsonData = await data.json();
        setRoles(jsonData);
      }
    })
  }

  const fetchUserRoles = async () => {
    await fetch('/api/role/getalluserroles')
    .then(async (data) => {
      if (data.ok) {
        const jsonData = await data.json();
        setUserRoles(jsonData);
      }
    })
  }

  const fetchUserRoleUpdate = async (userId, roleName, newRoleStatus) => {
    const formData = new FormData();
    formData.append('model.UserId', userId);
    formData.append('model.RoleName', roleName);
    formData.append('model.RoleStatus', newRoleStatus);


    let returnRoleStatus = !newRoleStatus;
    await fetch('/api/role/update', {
      method: "PUT",
      body: formData,
    })
    .then(async (data) => {
      if (data.ok) {
        const responseObj = await data.json();
        returnRoleStatus = responseObj.roleStatus;
      } else {
        returnRoleStatus = !newRoleStatus;
      }
    })

    return returnRoleStatus;
  }

  const handleRoleChange = async (event, userId, roleName) => {
    event.preventDefault();
    event.target.disabled = true;
    const roleStatus = await fetchUserRoleUpdate(userId, roleName, event.target.checked);
    event.target.checked = roleStatus;
    event.target.disabled = false;
  }

  useEffect(() => {
    fetchRoles();
    fetchUserRoles()
  }, [])

  return (
    <Table responsive size="sm">
      <thead>
        <tr align="center">
          <th>#</th>
          <th>Username</th>
          {roles.map((role, index) => (
            <th key={index + 2}>
              {role}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {userRoles.map((user, index) => (
          <tr key={index} align="center">
            <th scope="row">{index + 1}</th>
            <td key={0}>{user.userName}</td>
            {roles.map((role, i) => (
              <td key={i + 1}>
                <Input
                  type="checkbox"
                  defaultChecked={user.roles.includes(role)}
                  onChange={(e) => handleRoleChange(e, user.userId, role)}
                />
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </Table>
  );
}
