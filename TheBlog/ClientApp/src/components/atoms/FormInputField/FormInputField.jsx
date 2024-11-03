import { FormGroup, Input, Label } from 'reactstrap';

export default function FormInputField({ id, name, label, type, required, args}) {
  return (
    <FormGroup floating>
      <Input
        id={id}
        name={name}
        placeholder={label}
        type={type}
        className="rounded-3 userForm__input"
        required={required}
        bsSize="sm"
        {...args}
      />
      <Label for={id} className="userForm__label">
        {label}
      </Label>
    </FormGroup>
  );
}
