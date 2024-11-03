import './FormInputPill.scss';
import { FormGroup, Input, Label } from 'reactstrap';

export default function FormInputPill({ id, name, label, type, required, args}) {
  return (
    <FormGroup floating>
      <Input
        id={id}
        name={name}
        placeholder={label}
        type={type}
        className="rounded-pill userForm__input"
        required={required}
        {...args}
      />
      <Label for={id} className="userForm__label">
        {label}
      </Label>
    </FormGroup>
  );
}
