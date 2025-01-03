import React, { useEffect, useState } from 'react'
import "./styles.css"
import { Button, Col, Form, FormGroup, Input, Label, Row } from 'reactstrap'
//@ts-ignore
import userLogo from "../assets/user.svg"
import { Link, useNavigate } from 'react-router-dom'
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useAuth } from '../context/AuthContext'


const validationSchema = Yup.object({
    name: Yup.string(),
    surname: Yup.string(),
    email: Yup.string(),
    age: Yup.string(),
    address: Yup.string(),
    password: Yup.string()
});


export default function UserRegister() {

    const navigate = useNavigate()

    const { userRegister } = useAuth()

    const formData = useFormik({
        initialValues: {
            name: "",
            surname: "",
            email: "",
            age: "",
            address: "",
            password: "",

        },
        validationSchema: validationSchema,
        onSubmit: async (values) => {
            console.log(values)
            await userRegister(values)
            navigate("/dashboard")
        }
    });

    return (
        <div className='login-page'>
            <div className='login-container'>
                <div className='login-header'>
                    <h2>Sing-up User</h2>
                    <div className='usr-div'>
                        <img src={userLogo} width={70} height={85} />
                    </div>
                </div>
                <div className='form-container'>
                    <Form onSubmit={formData.handleSubmit}>
                        <FormGroup>
                            <Label className='ms-1'>Name</Label>
                            <Input
                                id='name'
                                name="name"
                                type='text'
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.name}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label className='ms-1'>Surname</Label>
                            <Input
                                id="surname"
                                name="surname"
                                type="text"
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.surname}
                            />
                        </FormGroup>


                        <FormGroup>
                            <Label className='ms-1'>Address</Label>
                            <Input
                                id='address'
                                name="address"
                                type='text'
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.address}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label className='ms-1'>Age</Label>
                            <Input
                                id='age'
                                name="age"
                                type='text'
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.age}
                            />
                        </FormGroup>

                        <FormGroup>
                            <Label className='ms-1'>Email</Label>
                            <Input
                                id='email'
                                name="email"
                                type='text'
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.email}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label className='ms-1'>Password</Label>
                            <Input
                                id='password'
                                name="password"
                                type='password'
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.password}
                            />
                        </FormGroup>
                        {formData.touched.password && formData.errors.password ? (
                            <div style={{ color: "red", textAlign: "center" }}>{formData.errors.password}</div>
                        ) : null}
                        <div className='mt-4' style={{ width: "100%" }}>

                            <Row>
                                <Col>
                                    <Link className="link-component my-4" to="/doctor-login">Doctor Login</Link><br />
                                    <Link className="link-component my-4" to="/doctor-register">Doctor Sing-up</Link>
                                </Col>
                                <Col>
                                    <Link className="link-component my-4" to="/">User Login</Link>
                                    {/* <Link className="link-component my-4" to="/user-register">User Sign-up</Link> */}
                                </Col>
                            </Row>
                        </div>

                        <div className='btn-div'>
                            <Button outline className='login-btn' type='submit'>
                                Sign up
                            </Button>
                        </div>

                    </Form>
                </div>

            </div >
        </div >
    )
}
